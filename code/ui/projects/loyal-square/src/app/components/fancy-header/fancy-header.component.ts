import { Component, Input, OnInit } from '@angular/core';
import { ThemeService } from '../../services/themes/theme.service';
import { ThemeStructure } from '../../services/themes/theme.types';
import { Classes } from 'jss';

@Component({
  selector: 'app-fancy-header',
  templateUrl: './fancy-header.component.html',
  styleUrls: ['./fancy-header.component.css'],
})
export class FancyHeaderComponent implements OnInit {
  @Input({required: true}) headingText: string = 'Hello';
  public classes: Classes<keyof ThemeStructure> | undefined;
  constructor(private themeService: ThemeService) {}
  ngOnInit(): void {
    this.classes = this.themeService.themeClasses;
  }
}
