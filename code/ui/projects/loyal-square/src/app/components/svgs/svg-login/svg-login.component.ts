import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-svg-login',
  templateUrl: './svg-login.component.svg',
  styleUrls: ['./svg-login.component.css']
})
export class SvgLoginComponent {
  @Input() primaryFill: string = "#0046CE";
  @Input() secondaryFill: string = "white";
  @Input() secondaryOpacity: string = "0.21";
}
