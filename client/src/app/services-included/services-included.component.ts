import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ServiceIncluded } from '../shared/models/birthdays/serviceincluded';
import { UserParams } from '../shared/models/myparams';
import { ServicesIncludedService } from './services-included.service';

type NewType = ToastrService;

@Component({
  selector: 'app-services-included',
  templateUrl: './services-included.component.html',
  styleUrls: ['./services-included.component.css']
})
export class ServicesIncludedComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  servicesIncluded: ServiceIncluded[];
  userParams: UserParams;
  totalCount: number;

  constructor(private servicesIncludedService: ServicesIncludedService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.servicesIncludedService.getUserParams();
     }

  ngOnInit(): void {
    this.getServicesIncluded();
  }

  getServicesIncluded() {
    this.servicesIncludedService.setUserParams(this.userParams);
    this.servicesIncludedService.getServicesIncluded(this.userParams)
    .subscribe(response => {
      this.servicesIncluded = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getServicesIncluded();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.servicesIncludedService.resetUserParams();
    this.getServicesIncluded();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.servicesIncludedService.setUserParams(this.userParams);
      this.getServicesIncluded();
    }
}

}
