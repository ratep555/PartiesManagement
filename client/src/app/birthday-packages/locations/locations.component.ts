import { Component, OnInit } from '@angular/core';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {
  locationList = [];

  constructor(private birthdayPackagesService: BirthdayPackagesService) { }

  ngOnInit(): void {
  this.birthdayPackagesService.getLocations().subscribe(res => this.locationList = res as []);
  }

}
