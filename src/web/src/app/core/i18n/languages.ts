/** Languages supported both as UI language and as learning language (see docs/adr/0006). */
export const SUPPORTED_LANGUAGES = ['pl', 'en', 'tr'] as const;

export type LanguageCode = (typeof SUPPORTED_LANGUAGES)[number];

export const DEFAULT_LANGUAGE: LanguageCode = 'pl';

export function isLanguageCode(value: unknown): value is LanguageCode {
  return SUPPORTED_LANGUAGES.some((lang) => lang === value);
}

/**
 * Each language in its own name (endonym), used by the UI-language switch so people can find
 * their language even if they can't read the current one. Constants, not translated
 * (docs/conventions/frontend.md, "Language names and tooltips").
 */
export const LANGUAGE_ENDONYMS: Record<LanguageCode, string> = {
  pl: 'Polski',
  en: 'English',
  tr: 'Türkçe',
};
