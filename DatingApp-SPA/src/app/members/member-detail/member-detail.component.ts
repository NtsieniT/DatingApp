import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  galleryOptions: NgxGalleryOptions[]; // Used to configure images 
  galleryImages: NgxGalleryImage[]; // Used for array of images to display

  constructor(private userService: UserService,
              private alertyfy: AlertifyService,
              private route: ActivatedRoute // Gets parameters to activated route
              ) { }

  ngOnInit() {
    // This gets data from our route instead of loading data directly from the service
    this.route.data.subscribe(data => {
     this.user = data.user;
    });

    // This is the configuration for NgxGallery for images
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false

    },
    // max-width 800
    {
      breakpoint: 800,
      width: '100%',
      height: '600px',
      imagePercent: 80,
      thumbnailsPercent: 20,
      thumbnailsMargin: 20,
      thumbnailMargin: 20
    },
    // max-width 400
    {
      breakpoint: 400,
      preview: false
    }
  ];

    // Assign array of images
    this.galleryImages = this.getImages();


  }

  getImages(){
    const imageUrls = [];

    for (let i = 0; i < this.user.photos.length; i++) {

      imageUrls.push({
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        description: this.user.photos[i].description,
      });

    }

    return imageUrls;
  }


 /*  loadUser(){

    this.userService.getUser(this.route.snapshot.params.id).subscribe((user: User) =>{
      this.user = user;
    },
    error => {
      this.alertyfy.error(error);
    });
  } */

}
