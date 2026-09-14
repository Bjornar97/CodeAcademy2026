/**
 * Kjøres én gang når Next-serveren starter.
 *
 * Eneste jobb: starte MSW og mock-baristaen hvis USE_MOCK_API=true, slik at
 * resten av appen kan være helt uvitende om at den ikke snakker med en ekte
 * container. Er mock-modus av, gjør denne fila ingenting.
 */
export async function register() {
  if (process.env.NEXT_RUNTIME !== "nodejs") return;
  if (process.env.USE_MOCK_API !== "true") return;

  const { setupServer } = await import("msw/node");
  const { handlers } = await import("@/mocks/handlers");
  const { startMockBarista } = await import("@/mocks/events");

  setupServer(...handlers).listen({ onUnhandledRequest: "bypass" });
  startMockBarista();

  console.log("[kaffebar-web] Mock-modus er PÅ — ingen containere brukes.");
}
