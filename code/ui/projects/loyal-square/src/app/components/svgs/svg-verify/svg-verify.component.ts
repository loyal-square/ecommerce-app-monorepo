import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-svg-verify',
  templateUrl: './svg-verify.component.svg',
  styleUrls: ['./svg-verify.component.css']
})
export class SvgVerifyComponent {
  @Input() primaryFill: string = "#4387FF";
  @Input() secondaryFill: string = "white";
}
