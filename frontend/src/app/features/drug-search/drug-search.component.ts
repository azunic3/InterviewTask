import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DrugApiService } from '../../services/drug-api.service';
import { FormsModule } from '@angular/forms';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-drug-search',
  templateUrl: './drug-search.component.html',
  styleUrls: ['./drug-search.component.scss'],
})
export class DrugSearchComponent implements OnInit {
  query = '';
  loading = false;
  error: string | null = null;
  result: any = null;
  allergensInput = ''; // npr "penicillin, sulfa"
  notifyEmail = '';
  notifyState: 'idle' | 'loading' | 'success' | 'exists' | 'error' = 'idle';
  notifyMessage = '';
  adverseState: 'idle' | 'loading' | 'success' | 'error' = 'idle';
  adverseMessage = '';
  adverseChartData: { term: string; count: number }[] = [];
  private adverseChart?: Chart;

  constructor(
    private route: ActivatedRoute,
    private drugApi: DrugApiService,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      if (params['q']) {
        this.query = params['q'];
        this.search();
      }
    });
  }

  loadTopAdverseEvents(): void {
    if (!this.result?.query) return;

    this.adverseState = 'loading';
    this.adverseMessage = '';
    this.adverseChartData = [];

    this.drugApi.getTopAdverseEvents(this.result.query, 10).subscribe({
      next: (res) => {
        this.adverseChartData = res?.items ?? [];
        if (!this.adverseChartData.length) {
          this.adverseState = 'error';
          this.adverseMessage = 'No adverse event data found.';
          return;
        }

        this.adverseState = 'success';

        // sačekaj da Angular nacrta <canvas>
        setTimeout(() => this.renderAdverseChart(), 0);
      },
      error: () => {
        this.adverseState = 'error';
        this.adverseMessage = 'Could not load side effects chart.';
      },
    });
  }

  private renderAdverseChart(): void {
    const canvas = document.getElementById(
      'adverseChart',
    ) as HTMLCanvasElement | null;
    if (!canvas) return;

    const labels = this.adverseChartData.map((x) => x.term);
    const values = this.adverseChartData.map((x) => x.count);

    if (this.adverseChart) {
      this.adverseChart.destroy();
    }

    this.adverseChart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'Reports',
            data: values,
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: true },
        },
        scales: {
          x: { ticks: { maxRotation: 60, minRotation: 0 } },
          y: { beginAtZero: true },
        },
      },
    });
  }

  search(): void {
    if (!this.query) return;

    this.loading = true;
    this.error = null;
    this.result = null;

    this.drugApi.searchDrug(this.query).subscribe({
      next: (res) => {
        this.result = res;
        this.loading = false;
        this.loadTopAdverseEvents();
      },
      error: () => {
        this.error = 'Failed to search drug.';
        this.loading = false;
      },
    });
  }

  get allergensList(): string[] {
    return this.allergensInput
      .split(',')
      .map((x) => x.trim())
      .filter(Boolean);
  }

  notifyMe(): void {
    if (!this.result?.drugKey) return;
    if (!this.notifyEmail) return;

    this.notifyState = 'loading';
    this.notifyMessage = '';

    this.drugApi
      .notifyAvailability(this.result.drugKey, this.notifyEmail)
      .subscribe({
        next: (res) => {
          this.notifyState = 'success';
          this.notifyMessage =
            res?.message ??
            'Thanks! We saved your request and will notify you when it’s available.';
        },
        error: (err) => {
          this.notifyState = 'error';
          this.notifyMessage = err?.error
            ? typeof err.error === 'string'
              ? err.error
              : err.error?.message
            : 'Could not save your request. Please try again.';
        },
      });
  }

  get allergyStatus() {
    const allergens = this.allergensList;
    if (!this.result || allergens.length === 0)
      return { type: 'none', matched: [] as string[] };

    const hay =
      `${this.result.safetyText ?? ''} ${this.result.drug?.activeIngredient ?? ''}`.toLowerCase();

    const matched = allergens.filter((a) => hay.includes(a.toLowerCase()));
    return { type: matched.length ? 'match' : 'safe', matched };
  }
}
