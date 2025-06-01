import { Component } from '@angular/core';
import {Dream, Dreams} from '../dreams';
import {MatPaginator, PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-dreams-list',
  imports: [
    MatPaginator
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

  constructor(private dreamsService: Dreams) {}

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
}
