import {Component} from '@angular/core';
import {Dream, Dreams} from '../../../services/dreams/dreams';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {RouterLink} from '@angular/router';
import {Loader} from '../../aditional/loader/loader';
import {CreateDreamPopUp} from '../create-dream-pop-up/create-dream-pop-up';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import { UserRoles } from '../../../environment/UserRoles';


@Component({
  selector: 'app-dreams-list',
  imports: [
    MatPaginator,
    RouterLink,
    Loader,
    MatDialogModule,
  ],
  templateUrl: './dreams-list.html',
  styleUrl: './dreams-list.scss'
})

export class DreamsList {
  dreams: Dream[] =[];
  page: number = 0;
  size: number = 5;
  count: number = 1;
  loading: boolean = true;
  error = '';
  role: UserRoles | null = null;

  constructor(
    private dreamsService: Dreams,
    private dialog: MatDialog
  ) {
    const userInfo = localStorage.getItem('UserInfo');
    if(userInfo){
      this.role = +JSON.parse(userInfo).role as UserRoles;
    }
  }

  ngOnInit(){
    this.loadDreams();
  };

  loadDreams(){
    this.loading = true;

    this.dreamsService.getDreamsCount().subscribe({
      next: data => {
        this.count = data;
      }
    })

    this.dreamsService.getDreams(this.page,this.size).subscribe({
      next: data => {
        this.dreams = data;
        this.loading = false;
      },
      error: error => {
        this.error = error;
        this.loading = false;
        console.log(error);
      }
    });
  }

  onPageChange(event: PageEvent) {
    this.page = event.pageIndex;
    this.size = event.pageSize;
    this.loadDreams()
  }

  onCreateDreamClick(){
    const dialogRef = this.dialog.open(CreateDreamPopUp);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadDreams();
      }
    });
  }
  
  protected readonly UserRoles = UserRoles;
}
