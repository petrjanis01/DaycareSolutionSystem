import { Component, OnInit } from '@angular/core';
import { ClientsService, ClientBasicsDTO } from 'src/app/api/generated';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {
  public clientBasics: ClientBasicsDTO[];
  public clientNameFilter: string;
  public clientNameText: string;

  constructor(private clientService: ClientsService, private router: Router) { }

  async ngOnInit() {
    let dtos = await this.clientService.apiClientsAllClientBasicsGet();

    dtos.forEach(dto => {
      if (dto.profilePicture.pictureUri == null) {
        dto.profilePicture.pictureUri = 'assets/img/user-anonymous.png';
      }
    });

    this.clientBasics = dtos;
  }

  public openClientDetail(id: string) {
    this.router.navigate(['clients/detail', id]);
  }

  public addNewClient() {
    this.router.navigate(['clients/detail', 0])
  }
}
