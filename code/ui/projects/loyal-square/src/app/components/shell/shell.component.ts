import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/themes/theme.service';
import { Classes } from 'jss';
import { ThemeStructure } from '../../services/themes/theme.types';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.css'],
})
export class ShellComponent implements OnInit {
  public classes: Classes<keyof ThemeStructure> | undefined;
  constructor(public themeService: ThemeService) {}
  ngOnInit(): void {
    this.classes = this.themeService.themeClasses;
  }
}
