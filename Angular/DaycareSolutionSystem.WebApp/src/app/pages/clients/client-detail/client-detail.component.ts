import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientDTO, ClientsService } from 'src/app/api/generated';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.scss']
})
export class ClientDetailComponent implements OnInit {
  public client: ClientDTO;

  constructor(private route: ActivatedRoute, private clientsService: ClientsService) { }

  async ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');

    let dto = await this.clientsService.apiClientsSingleClientGet(id);
    if (dto.profilePicture.pictureUri == null) {
      dto.profilePicture.pictureUri = './../../../../assets/img/user-anonymous.png';
    }

    this.client = dto;
    console.log(this.client);
  }

}
