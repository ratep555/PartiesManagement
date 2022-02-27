import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BirthdayPackage } from 'src/app/shared/models/birthdays/birthdaypackage';
import { MyParams } from 'src/app/shared/models/myparams';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-birtday-packages-admin',
  templateUrl: './birtday-packages-admin.component.html',
  styleUrls: ['./birtday-packages-admin.component.css']
})
export class BirtdayPackagesAdminComponent implements OnInit {
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
