import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../environment/environment';

export interface Dream {
  id : string;
  title: string;
  description: string;
  producerId: string | null;
  rating: number;
  imageBase64: string;
  imageContentType: string;
  categories: CategoryResponseInDream[];
}

export interface CategoryResponseInDream {
  categoryId: string;
  title: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class Dreams {
  private urlSuffix = 'Dream';
  constructor(private http: HttpClient) { }

  getDreams(page:number, size:number): Observable<Dream[]> {
    return this.http.get<Dream[]>(`${environment.apiUrl}${this.urlSuffix}?skip=${page*size}&size=${size}`);
  }

  getDreamsCount(): Observable<number>{
    return this.http.get<number>(`${environment.apiUrl}${this.urlSuffix}/count`)
  }

  getDream(dreamId: string): Observable<Dream> {
    return this.http.get<Dream>(`${environment.apiUrl}${this.urlSuffix}/${dreamId}`);
  }

  deleteDream(dreamId: string) {
    return this.http.delete(`${environment.apiUrl}${this.urlSuffix}/${dreamId}`);
  }

  addDream(dream: FormData) : Observable<Object> {
    return this.http.post(`${environment.apiUrl}${this.urlSuffix}`, dream);
  }
}
