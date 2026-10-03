---
name: quiz-me
description: Quizzes the author on concepts that have come up in the project (docs/learning/journal.md and concepts/). Use when the author says "quiz", "przepytaj mnie", "sprawdź mnie", or for the weekly review.
---

# Learning journal quiz

1. Read `docs/learning/journal.md` (and optionally files from `docs/learning/concepts/`). If the author gave a topic in the arguments, narrow it down to that. With the argument `roadmap`, take the topics marked `[~]` in `docs/learning/roadmap.md` instead, and prefer "which option would you pick for WordWise and why?" and "what breaks in production if…" questions.
2. Pick **5 questions**: prefer the oldest topics and those with an empty "In my own words" (spaced repetition). Mix the types:
   - "explain in your own words",
   - "what happens when…" (predict),
   - "find the bug in this snippet" (short C# snippet),
   - "which file in WordWise is responsible for this?".
3. Ask **one question at a time** and wait for the answer. Don't give the answers up front.
4. After each answer: rating (✅ / 🟡 / ❌), 1–2 sentences of correction, a docs link (`microsoft-learn` MCP).
5. At the end: the score, 1–2 topics to review; add a `## YYYY-MM-DD — Quiz` entry to `docs/learning/journal.md` with the score and weak topics.
