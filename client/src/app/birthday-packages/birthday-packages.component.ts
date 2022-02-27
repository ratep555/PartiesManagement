import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BirthdayPackage } from '../shared/models/birthdays/birthdaypackage';
import { MyParams } from '../shared/models/myparams';
import { BirthdayPackagesService } from './birthday-packages.service';

@Component({
  selector: 'app-birthday-packages',
  templateUrl: './birthday-packages.component.html',
  styleUrls: ['./birthday-packages.component.css']
})
export class BirthdayPackagesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  birthdayPackages: BirthdayPackage[];
  myParams: MyParams;
  totalCount: number;

  constructor(private birthdayPackageService: BirthdayPackagesService) {
    this.myParams = this.birthdayPackageService.getMyParams();
   }

  ngOnInit(): void {
    this.getBirthdayPackages();
  }

  getBirthdayPackages() {
    this.birthdayPackageService.getBirthdayPackages().subscribe(response => {
      this.birthdayPackages = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.birthdayPackageService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.birthdayPackageService.setMyParams(params);
      this.getBirthdayPackages();
    }
  }

  onSearch() {
    const params = this.birthdayPackageService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.birthdayPackageService.setMyParams(params);
    this.getBirthdayPackages();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.birthdayPackageService.setMyParams(this.myParams);
    this.getBirthdayPackages();
  }
}



