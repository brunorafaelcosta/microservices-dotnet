import { Component, OnInit } from '@angular/core';

import { LayoutService } from '../../../layout';

@Component({
  selector: 'error-403-root',
  templateUrl: './error-403.component.html',
  styleUrls: ['./error-403.component.sass'],
})
export class Error403Component implements OnInit {
  private title: string = 'Error 403';

  constructor(private layoutService: LayoutService) {
  }

  ngOnInit(): void {
    this.layoutService.setTitle(this.title);
  }
}
