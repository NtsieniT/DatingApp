﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            // Setting up cloudinary account so that we can upload photos to the cloudinary account
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            // set up a new account for cloudinary in the Account constractor
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
                );

            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name ="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            // Check if the user attempting to upload their photo in their profile matches the token that the server is recieving

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))            
                return Unauthorized();            

            var userFromRepo = await _repo.GetUser(userId);


            var file = photoForCreationDto.File; // file to upload
            var uploadResult = new ImageUploadResult(); // used to store results we get back from cloudinary

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    // Give cloudinary upload params
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // Map our photo DTO into our photo itself
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            // If photo they are uploading is the first photo, set the photo as main

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);

            // Get photo from repo using GetPhoto method with the HTTPGET Name called GetPhoto
            // Map the photo to return from the PhotoForReturnDto

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto",new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            // check if id matches id of the user in the repository

            var user = await _repo.GetUser(userId);

            // make sure photo exists in the photos collection
            if (!user.Photos.Any(predicate => predicate.Id == id))
            {
                // if id does not match any of the photos id collection, return unauthorized
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            // Check if the photo is the main photo
            if (photoFromRepo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }

            // Get Current photo which is set to the main photo
            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

            // Set the photo to false changing it not to be the main photo
            currentMainPhoto.IsMain = false;

            // set the new uploaded photo to be the main photo
            photoFromRepo.IsMain = true;

            // Save all changes in the database
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("Could not set photo to main");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            // check if id matches id of the user in the repository

            var user = await _repo.GetUser(userId);

            // make sure photo exists in the photos collection
            if (!user.Photos.Any(predicate => predicate.Id == id))
            {
                // if id does not match any of the photos id collection, return unauthorized
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            // Check if the photo is the main photo
            if (photoFromRepo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }

            // Check if photo from repository has a public Id
            if (photoFromRepo.PublicId != null)
            {
                // Generate params to delete on cloudinary
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var results = _cloudinary.Destroy(deleteParams);

                if (results.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }

            }

            if (photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }
          

            if (await _repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("failed to delete the photo");


        }
      
    }
}
