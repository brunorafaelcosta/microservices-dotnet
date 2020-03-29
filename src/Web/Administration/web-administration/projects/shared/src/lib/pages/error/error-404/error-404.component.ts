import { Component, OnInit } from '@angular/core';

import { LayoutService } from '../../../layout';

@Component({
  selector: 'error-404-root',
  templateUrl: './error-404.component.html',
  styleUrls: ['./error-404.component.sass'],
})
export class Error404Component implements OnInit {
  private title: string = 'Error 404';

  constructor(private layoutService: LayoutService) {
  }

  ngOnInit(): void {
    this.layoutService.setTitle(this.title);
  }
}
