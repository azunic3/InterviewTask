import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DrugApiService, PopularSearch } from '../../services/drug-api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  popularSearches: PopularSearch[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private drugApi: DrugApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadPopularSearches();
  }

  loadPopularSearches(): void {
    this.loading = true;
    this.drugApi.getPopularSearches(5).subscribe({
      next: (data) => {
        this.popularSearches = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load popular searches.';
        this.loading = false;
      }
    });
  }

  goToSearch(query: string): void {
    this.router.navigate(['/search'], {
      queryParams: { q: query }
    });
  }
}
