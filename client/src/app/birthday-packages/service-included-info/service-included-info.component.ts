import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ServiceIncluded } from 'src/app/shared/models/birthdays/serviceincluded';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-service-included-info',
  templateUrl: './service-included-info.component.html',
  styleUrls: ['./service-included-info.component.css']
})
export class ServiceIncludedInfoComponent implements OnInit {
  serviceIncluded: ServiceIncluded;
  videoClipURL: SafeResourceUrl;

  constructor(private birthdayPackagesService: BirthdayPackagesService,
              private activatedRoute: ActivatedRoute,
              private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.loadServiceIncluded();
  }

  loadServiceIncluded() {
    return this.birthdayPackagesService.getServiceIncludedById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.serviceIncluded = response;
    this.videoClipURL = this.generateYoutubeURLForEmbeddedVideo(this.serviceIncluded.videoClip);
    }, error => {
    console.log(error);
    });
    }

    generateYoutubeURLForEmbeddedVideo(url: any): SafeResourceUrl{
      if (!url){
        return '';
      }
      let videoId = url.split('v=')[1];
      const ampersandPosition = videoId.indexOf('&');
      if (ampersandPosition !== -1){
        videoId = videoId.substring(0, ampersandPosition);
      }
      return this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${videoId}`);
    }


}
