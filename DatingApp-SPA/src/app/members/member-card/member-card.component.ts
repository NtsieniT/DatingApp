import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';


// This member card component is child component of member-list components
// We pass our user to the card component so that we display contents to this component


@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User; // From our parent component
  constructor() { }

  ngOnInit(): void {
  }

}
