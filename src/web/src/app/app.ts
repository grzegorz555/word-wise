import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterOutlet } from '@angular/router';
import { TranslocoPipe, TranslocoService } from '@jsverse/transloco';
import { LanguageCode, SUPPORTED_LANGUAGES } from './core/i18n/languages';

@Component({
  imports: [RouterOutlet, TranslocoPipe],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  private readonly transloco = inject(TranslocoService);

  protected readonly languages = SUPPORTED_LANGUAGES;
  protected readonly activeLang = toSignal(this.transloco.langChanges$, {
    initialValue: this.transloco.getActiveLang(),
  });

  protected setLanguage(lang: LanguageCode): void {
    this.transloco.setActiveLang(lang);
  }
}
