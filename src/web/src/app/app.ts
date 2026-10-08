import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterOutlet } from '@angular/router';
import { TranslocoPipe, TranslocoService } from '@jsverse/transloco';
import { HlmToggleGroupImports } from '@spartan-ng/helm/toggle-group';
import { HlmTooltipImports } from '@spartan-ng/helm/tooltip';
import {
  isLanguageCode,
  LANGUAGE_ENDONYMS,
  languageSwitchLabel,
  SUPPORTED_LANGUAGES,
} from './core/i18n/languages';

@Component({
  imports: [RouterOutlet, TranslocoPipe, HlmToggleGroupImports, HlmTooltipImports],
  selector: 'app-root',
  templateUrl: './app.html',
  host: { class: 'block max-w-4xl p-4 mx-auto' },
})
export class App {
  private readonly transloco = inject(TranslocoService);

  protected readonly languages = SUPPORTED_LANGUAGES;
  protected readonly endonyms = LANGUAGE_ENDONYMS;
  protected readonly switchLabel = languageSwitchLabel;
  protected readonly activeLang = toSignal(this.transloco.langChanges$, {
    initialValue: this.transloco.getActiveLang(),
  });

  protected setLanguage(lang: unknown): void {
    if (!isLanguageCode(lang)) {
      return;
    }

    this.transloco.setActiveLang(lang);
  }
}
