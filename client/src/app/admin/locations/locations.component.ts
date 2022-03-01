import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Location1 } from 'src/app/shared/models/location';
import { UserParams } from 'src/app/shared/models/myparams';
import { LocationsService } from './locations.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  locations: Location1[];
  userParams: UserParams;
  totalCount: number;

  constructor(private locationsService: LocationsService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.locationsService.getUserParams();
     }

  ngOnInit(): void {
    this.getLocations();
  }

  getLocations() {
    this.locationsService.setUserParams(this.userParams);
    this.locationsService.getLocations(this.userParams)
    .subscribe(response => {
      this.locations = response.data;
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
    this.getLocations();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.locationsService.resetUserParams();
    this.getLocations();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.locationsService.setUserParams(this.userParams);
      this.getLocations();
    }
}

}
