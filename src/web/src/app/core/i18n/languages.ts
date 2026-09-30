/** Languages supported both as UI language and as learning language (see docs/adr/0006). */
export const SUPPORTED_LANGUAGES = ['pl', 'en', 'tr'] as const;

export type LanguageCode = (typeof SUPPORTED_LANGUAGES)[number];

export const DEFAULT_LANGUAGE: LanguageCode = 'pl';
