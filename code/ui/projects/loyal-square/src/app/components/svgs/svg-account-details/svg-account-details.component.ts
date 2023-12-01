import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-svg-account-details',
  templateUrl: './svg-account-details.component.svg',
  styleUrls: ['./svg-account-details.component.css']
})
export class SvgAccountDetailsComponent {
  @Input() primaryFill: string = "#4387FF";
  @Input() secondaryFill: string = "#84B0FF";
}
