import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Marker } from 'leaflet';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BirthdayPackagesService } from 'src/app/birthday-packages/birthday-packages.service';
import { CoordinatesMap, CoordinatesMapWithMessage } from 'src/app/shared/models/coordinate';
import { Location1 } from 'src/app/shared/models/location';
import { jsonSchema } from 'uuidv4';
import { LocationsService } from '../locations.service';

@Component({
  selector: 'app-edit-location',
  templateUrl: './edit-location.component.html',
  styleUrls: ['./edit-location.component.css']
})
export class EditLocationComponent implements OnInit {

  model: Location1;
  locationForm: FormGroup;
  countryList = [];
  id: number;
  initialCoordinates: CoordinatesMap[] = [];
  coordinates: CoordinatesMapWithMessage[] = [];
  layers: Marker<any>[] = [];

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private locationsService: LocationsService,
              private birthdayPackageService: BirthdayPackagesService,
              private accountService: AccountService) { }

ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];
  /* this.activatedRoute.params.subscribe(params => {
    this.birthdayPackageService.getLocation(params.id).subscribe(
      location => this.model = location);
    this.coordinates = [{latitude: location.latitude, longitude: location.longitude, message: location.street}];
  });
 */
  this.activatedRoute.params.subscribe((params) => {
    this.birthdayPackageService.getLocation(params.id).subscribe((location) => {
      console.log(location);
      this.model = location;
      this.coordinates = [{latitude: location.latitude, longitude: location.longitude, message: location.street}];
      console.log(this.coordinates);
    });
  });

  this.accountService.getCountries()
    .subscribe(res => this.countryList = res as []);

  this.locationForm = this.fb.group({
    id: [this.id],
    street: [null, [Validators.required,
      Validators.minLength(2), Validators.maxLength(40)]],
    city: [null, [Validators.required,
      Validators.minLength(2), Validators.maxLength(40)]],
    email: [null, [Validators.required,
      Validators.minLength(3), Validators.maxLength(60)]],
    phone: [null, [Validators.required,
      Validators.minLength(3), Validators.maxLength(60)]],
    workingHours: [null, [Validators.required,
      Validators.minLength(3), Validators.maxLength(60)]],
    description: ['', [Validators.required,
      Validators.minLength(10), Validators.maxLength(2000)]],
    longitude: ['', [Validators.required]],
    latitude: ['', [Validators.required]],
    countryId: [null]
  });


  this.birthdayPackageService.getLocation(this.id)
  .pipe(first())
  .subscribe(x => this.locationForm.patchValue(x));

}

onSubmit() {
  if (this.locationForm.invalid) {
      return;
  }
  this.updateLocation();
}

private updateLocation() {
this.locationsService.updateLocation(this.id, this.locationForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('locations');
      }, error => {
        console.log(error);
      });
    }

 onSelectedLocation(coordinates: CoordinatesMap) {
  this.locationForm.patchValue(coordinates);
  this.initialCoordinates.push({latitude: this.model.latitude,
    longitude: this.model.longitude});
   }

}
