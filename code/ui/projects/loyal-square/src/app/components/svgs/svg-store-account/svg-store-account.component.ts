import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-svg-store-account',
  templateUrl: './svg-store-account.component.svg',
  styleUrls: ['./svg-store-account.component.css']
})
export class SvgStoreAccountComponent {
  @Input() primaryFill: string = "#4387FF";
  @Input() secondaryFill: string = "#84B0FF";
}
