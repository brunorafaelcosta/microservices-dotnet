import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'private-component',
  template: `<p>I'm the private content!</p>`,
})
export class PrivateComponent implements OnInit {
  constructor() {
  }

  ngOnInit(): void {
  }
}
