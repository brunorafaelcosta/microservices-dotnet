import { Component, OnInit } from '@angular/core';

import { LayoutService } from '../../../layout';

@Component({
  selector: 'error-500-root',
  templateUrl: './error-500.component.html',
  styleUrls: ['./error-500.component.sass'],
})
export class Error500Component implements OnInit {
  private title: string = 'Error 500';

  constructor(private layoutService: LayoutService) {
  }

  ngOnInit(): void {
    this.layoutService.setTitle(this.title);
  }
}
