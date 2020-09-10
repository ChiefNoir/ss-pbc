import { Component, Input, OnInit } from '@angular/core';
import { StaticNames } from 'src/app/common/StaticNames';
import { MessageDescription, MessageType } from '../message/message.component';
import { GalleryImage } from 'src/app/model/GalleryImage';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
})

export class CarouselComponent implements OnInit
{
  @Input()
  public images: Array<GalleryImage>;

  public selectedImage: GalleryImage;
  public currentImageIndex: number = 0;

  public loading: boolean = true;
  public message: MessageDescription = {text: StaticNames.LoadInProgress, type: MessageType.Spinner };


  public ngOnInit(): void
  {
    this.currentImageIndex = 0;
    this.selectedImage = this.images[this.currentImageIndex];
  }

  public onLoad(): void
  {
    this.loading = false;
  }

   public changeImage(value: number): void
   {
     const lastIndex = this.currentImageIndex;

     this.currentImageIndex = this.currentImageIndex + value;

     if (this.currentImageIndex === -1)
     {
       this.currentImageIndex = this.images.length - 1;
     }
     if (this.currentImageIndex === this.images.length)
     {
       this.currentImageIndex = 0;
     }

     if (lastIndex === this.currentImageIndex) { return; }

     this.loading = true;
     this.selectedImage = this.images[this.currentImageIndex];
   }
}

