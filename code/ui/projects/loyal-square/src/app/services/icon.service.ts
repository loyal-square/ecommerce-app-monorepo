import { Injectable } from '@angular/core';
import { MatIconRegistry } from "@angular/material/icon";
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class IconService {

  constructor(private iconRegistry: MatIconRegistry, private domSanitizer: DomSanitizer) { }

  private baseLocation: string = '/assets/images/icons';

  init() {
    this.registerCommonIcons();
    this.registerSearchIcons();
    this.registerSocialIcons();
    this.registerStoreIcons();
  }

  addSvgIcon(name: string, location: string) {
    this.iconRegistry.addSvgIcon(
      name,
      this.domSanitizer.bypassSecurityTrustResourceUrl(location)
    );
  }

  registerCommonIcons() {
    this.addSvgIcon("add", `${this.baseLocation}/common/Add.svg`);
    this.addSvgIcon("brightness", `${this.baseLocation}/common/Brightness.svg`);
    this.addSvgIcon("check-circle", `${this.baseLocation}/common/Check-circle.svg`);
    this.addSvgIcon("check", `${this.baseLocation}/common/Check.svg`);
    this.addSvgIcon("close", `${this.baseLocation}/common/Close.svg`);
    this.addSvgIcon("info", `${this.baseLocation}/common/Info.svg`);
    this.addSvgIcon("remove", `${this.baseLocation}/common/Remove.svg`);
    this.addSvgIcon("security", `${this.baseLocation}/common/Security.svg`);

    this.addSvgIcon("arrow-up", `${this.baseLocation}/common/Arrow-up.svg`);
    this.addSvgIcon("arrow-right", `${this.baseLocation}/common/Arrow-right.svg`);
    this.addSvgIcon("arrow-up-fill", `${this.baseLocation}/common/Arrow-up-fill.svg`);
    this.addSvgIcon("arrow-down-fill", `${this.baseLocation}/common/Arrow-down-fill.svg`);
    this.addSvgIcon("arrow-left-fill", `${this.baseLocation}/common/Arrow-left-fill.svg`);
    this.addSvgIcon("arrow-right-fill", `${this.baseLocation}/common/Arrow-right-fill.svg`);
    this.addSvgIcon("arrow-up-key", `${this.baseLocation}/common/Arrow-up-key.svg`);
    this.addSvgIcon("arrow-down-key", `${this.baseLocation}/common/Arrow-down-key.svg`);
    this.addSvgIcon("arrow-left-key", `${this.baseLocation}/common/Arrow-left-key.svg`);
    this.addSvgIcon("arrow-right-key", `${this.baseLocation}/common/Arrow-right-key.svg`);
  }

  registerSearchIcons() {
    this.addSvgIcon("date-range", `${this.baseLocation}/search/Date-range.svg`);
    this.addSvgIcon("search", `${this.baseLocation}/search/Search.svg`);
  }

  registerSocialIcons() {
    this.addSvgIcon("email", `${this.baseLocation}/socials/Email.svg`);
    this.addSvgIcon("facebook", `${this.baseLocation}/socials/Facebook.svg`);
    this.addSvgIcon("instagram", `${this.baseLocation}/socials/Instagram.svg`);
  }

  registerStoreIcons() {
    this.addSvgIcon("add-store", `${this.baseLocation}/store/Add-store.svg`);
    this.addSvgIcon("camera", `${this.baseLocation}/store/Camera.svg`);
    this.addSvgIcon("heart-fill", `${this.baseLocation}/store/Heart-fill.svg`);
    this.addSvgIcon("heart-outline", `${this.baseLocation}/store/Heart-outline.svg`);
    this.addSvgIcon("hide", `${this.baseLocation}/store/Hide.svg`);
    this.addSvgIcon("library-add", `${this.baseLocation}/store/Library-add.svg`);
    this.addSvgIcon("more", `${this.baseLocation}/store/More.svg`);
    this.addSvgIcon("notice", `${this.baseLocation}/store/Notice.svg`);
    this.addSvgIcon("profit", `${this.baseLocation}/store/Profit.svg`);
    this.addSvgIcon("sell", `${this.baseLocation}/store/Sell.svg`);
    this.addSvgIcon("settings", `${this.baseLocation}/store/Settings.svg`);
    this.addSvgIcon("shine", `${this.baseLocation}/store/Shine.svg`);
    this.addSvgIcon("star-half", `${this.baseLocation}/store/Star-half.svg`);
    this.addSvgIcon("star-outline", `${this.baseLocation}/store/Star-outline.svg`);
    this.addSvgIcon("star-fill", `${this.baseLocation}/store/Star-fill.svg`);
    this.addSvgIcon("store", `${this.baseLocation}/store/Store.svg`);
    this.addSvgIcon("tier", `${this.baseLocation}/store/Tier.svg`);
    this.addSvgIcon("trash-outline", `${this.baseLocation}/store/Trash-outline.svg`);
    this.addSvgIcon("view", `${this.baseLocation}/store/View.svg`);
  }
}
