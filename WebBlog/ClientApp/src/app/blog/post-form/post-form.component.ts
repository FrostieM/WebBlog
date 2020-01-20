import {Component, EventEmitter, Inject, Input, Output} from '@angular/core';
import { Router } from "@angular/router";
import {NgForm} from "@angular/forms";
import {ServerService} from "../../shared/services/server.service";

@Component({
  selector: 'blog-post-form-component',
  host: {
    class: ""
  },
  templateUrl: './post-form.component.html'
})
export class PostFormComponent{

  @Input() public type: string;

  @Output() public messageToUpdate = new EventEmitter();
  @Output() public messageToClose = new EventEmitter();

  public file;

  constructor(private router: Router,
              @Inject("BASE_URL") private baseUrl: string,
              private serverService: ServerService) {
  }

  setPosts(ngForm: NgForm) {
    let formData = new FormData();

    for(let key of Object.keys(ngForm.value)){
      formData.append(key, ngForm.value[key]);
    }

    formData.append("file", this.file);

    this.serverService.savePost(formData).subscribe(() => {
      this.messageToUpdate.emit();
    }, err => {
      console.log(err)
    });
  }

  public changeFile(file){
    this.file = file;
  }

  public closeForm(){
    this.messageToClose.emit();
  }


}
