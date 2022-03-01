import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CoordinatesMapWithMessage } from 'src/app/shared/models/coordinate';
import { Location1 } from 'src/app/shared/models/location';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-location-info',
  templateUrl: './location-info.component.html',
  styleUrls: ['./location-info.component.css']
})
export class LocationInfoComponent implements OnInit {
  location: Location1;
  coordinates: CoordinatesMapWithMessage[] = [];


  constructor(private birthdayPackagesService: BirthdayPackagesService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.birthdayPackagesService.getLocation(params.id).subscribe((location) => {
        console.log(location);
        this.location = location;
        this.coordinates = [{latitude: location.latitude, longitude: location.longitude, message: location.street}];
        console.log(this.coordinates);
      });
    });
   // this.loadLocations();
  }

  /* loadLocations() {
    return this.birthdayPackagesService.getLocation(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.location = response;
    this.coordinates = [{latitude: this.location.latitude, longitude: this.location.longitude, message: this.location.street}];
    console.log(this.coordinates);

  }, error => {
    console.log(error);
    });
    } */


}
