---
name: spec-feature
description: Prowadzi autora przez napisanie specyfikacji nowej funkcji WordWise (docs/specs/NNN-nazwa/spec.md + tasks.md z trybami [solo]/[learn]/[gen]). Używaj przed implementacją każdej nowej funkcji lub gdy autor mówi "nowa funkcja", "napiszmy spec".
---

# Specyfikacja funkcji (lekki Spec-Driven Development)

1. Przeczytaj `docs/vision.md`, `docs/specs/README.md` i ADR-y związane z funkcją. Nie czytaj kodu, jeśli nie jest potrzebny.
2. Zadaj autorowi **max 3–4 pytania** o niejasności (zakres, reguły brzegowe, UI). Resztę zaproponuj jako domyślne.
3. Utwórz `docs/specs/NNN-slug/spec.md` wg wzoru `docs/specs/001-word-catalog/spec.md`:
   Cel · Zakres · Kontrakt (HTTP/UI) · Reguły R1…Rn · Kryteria akceptacji (checklista, testowalne) · Poza zakresem.
4. Utwórz `tasks.md`: 5–10 zadań, każde z trybem. Zasada podziału:
   - konfiguracja/boilerplate → `[gen]`,
   - logika domeny, zapytania EF, testy reguł → `[learn]` lub `[solo]`,
   - ~1/3 każdego trybu; przy każdym zadaniu linijka *„Do zrozumienia:”* z koncepcjami.
5. Nowa decyzja architektoniczna po drodze → skill `new-adr`.
6. Zaktualizuj tabelę w `docs/specs/README.md`. Autor zatwierdza tryby przed startem.
