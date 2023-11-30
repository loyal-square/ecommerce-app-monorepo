import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-svg-signup',
  templateUrl: './svg-signup.component.svg',
  styleUrls: ['./svg-signup.component.css']
})
export class SvgSignupComponent {
  @Input() primaryFill: string = "#4387FF";
  @Input() secondaryFill: string = "#84B0FF";
}
