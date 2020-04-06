import { Component, OnInit } from '@angular/core';

import { AuthenticationService, IIdentity } from '../../../authentication';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.sass'],
})
export class NavbarComponent implements OnInit {
  User: IIdentity; 

  constructor(
    private authentication: AuthenticationService,
  ) {
  }

  ngOnInit(): void {
    this.User = this.authentication.User;
  }

  logout(): void {
    debugger;
    this.authentication.Logout();
  }
}
