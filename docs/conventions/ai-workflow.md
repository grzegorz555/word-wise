# How we work with AI (Claude Code)

Method: lightweight **Spec-Driven Development** (inspired by GitHub Spec Kit) + the **Explore → Plan → Code → Verify** workflow from Claude Code best practices.

## Lifecycle of a feature
1. **Spec** (`docs/specs/NNN-name/spec.md`): what and why, acceptance criteria. Skill `/spec-feature`.
2. **Tasks** (`tasks.md`): small steps, each tagged with a mode (see below). The author decides the modes.
3. **Implementation** task by task, in plan mode for larger steps.
4. **Review:** the `code-reviewer` agent (fresh context) checks conformance with the conventions.
5. **Learning:** an entry in `docs/learning/journal.md` + `/quiz-me` every week.
6. `/clear` between tasks: context costs tokens.

## Task modes
| Mode | Who writes the code | Claude's role | "Done" condition |
|---|---|---|---|
| `[solo]` | the author | review, questions, links to docs | the author got the tests passing on their own |
| `[learn]` | the author (key parts) | skeleton + `TODO(human)`, hints instead of solutions | the author filled in the TODOs, tests green |
| `[gen]` | Claude | generates + explains | **explain-back**: the author describes the code in their own words |

Starting ratio: roughly 1/3 of each mode. Boilerplate and configuration are `[gen]`; domain logic and EF queries are `[learn]`/`[solo]`.

## Tooling in the repo
- The **Mentor** output style (`.claude/output-styles/mentor.md`) is enabled by default in `.claude/settings.json`. To change it: `/output-style`.
- Agents (`.claude/agents/`): `backend-mentor` (explains, guides Socratically), `code-reviewer` (assesses code against the conventions).
- Skills (`.claude/skills/`): `/spec-feature`, `/new-adr`, `/explain-this`, `/quiz-me`.
- MCP (`.mcp.json`): `microsoft-learn` (official .NET docs) and `angular-cli` (Angular docs + best practices). The source of links in explanations.
- Docs language: repo docs are in English; explanations to the author are in Polish. Every tracked doc has a local, git-ignored Polish copy (`*.local.md`, `CLAUDE.pl.local.md`, `.local/docs-pl/.claude/…`), updated together with the English doc (see rule 5a in `CLAUDE.md`).

## Self-discipline (challenge)
- **Predict before run:** before running anything, write down what will happen.
- **Explain-back** after every `[gen]`.
- **Tests first** in `[learn]`/`[solo]`.
- **No commit without understanding** every line of the diff.
- Once a week: `/quiz-me` + a review of the journal.
- Outside the project: the C# track on Exercism, `dotnet/eShop` as reference code (reading and comparing).

## Saving tokens
- CLAUDE.md is short; details live in `docs/` and are loaded only when needed.
- One spec = one folder. ADRs instead of long chat discussions.
- Delegate searches through larger code to subagents (Explore) to keep the context clean.
