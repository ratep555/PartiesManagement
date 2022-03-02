import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ServiceIncluded } from 'src/app/shared/models/birthdays/serviceincluded';
import { MyParams } from 'src/app/shared/models/myparams';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-services-included',
  templateUrl: './services-included.component.html',
  styleUrls: ['./services-included.component.css']
})
export class ServicesIncludedComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  servicesIncluded: ServiceIncluded[];
  myParams: MyParams;
  totalCount: number;

  constructor(private birthdayPackageService: BirthdayPackagesService) {
    this.myParams = this.birthdayPackageService.getMyParams();
   }

  ngOnInit(): void {
    this.getServicesIncluded();
  }

  getServicesIncluded() {
    this.birthdayPackageService.getServicesIncluded().subscribe(response => {
      this.servicesIncluded = response.data;
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
      this.getServicesIncluded();
    }
  }

  onSearch() {
    const params = this.birthdayPackageService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.birthdayPackageService.setMyParams(params);
    this.getServicesIncluded();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.birthdayPackageService.setMyParams(this.myParams);
    this.getServicesIncluded();
  }
}


