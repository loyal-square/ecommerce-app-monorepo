import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-svg-register',
  templateUrl: './svg-register.component.svg',
  styleUrls: ['./svg-register.component.css']
})
export class SvgRegisterComponent {
  @Input() primaryFill: string = "#0046CE";
  @Input() secondaryFill: string = "white";
  @Input() secondaryOpacity: string = "0.24";
}
