import { http, HttpResponse } from "msw";
import { KAFFEBAR_API_URL } from "@/lib/config";
import type { CreateOrderRequest, OrderStatus } from "@/types/domain";
import { emitMockOrderCreated, emitMockStatusChanged } from "./events";
import {
  createMockOrder,
  findMockOrder,
  listMockOrders,
  MOCK_MENU,
  setMockStatus,
} from "./store";

/**
 * MSW-handlere som svarer som kaffebar-api. De ligger på nøyaktig de samme
 * stiene, så INGEN kode i src/lib/api.ts trenger å vite at den er i mock-modus.
 */

const problem = (status: number, title: string, detail: string) =>
  HttpResponse.json(
    { type: `https://codeacademy.soprasteria.no/problems/mock`, title, status, detail },
    { status, headers: { "content-type": "application/problem+json" } },
  );

export const handlers = [
  http.get(`${KAFFEBAR_API_URL}/menu`, () => HttpResponse.json(MOCK_MENU)),

  http.get(`${KAFFEBAR_API_URL}/orders`, ({ request }) => {
    const url = new URL(request.url);
    const status = url.searchParams.get("status") as OrderStatus | null;
    const limit = Number(url.searchParams.get("limit") ?? 100);
    return HttpResponse.json(listMockOrders({ ...(status ? { status } : {}), limit }));
  }),

  http.post(`${KAFFEBAR_API_URL}/orders`, async ({ request }) => {
    const body = (await request.json()) as CreateOrderRequest;

    if (!body.customerName?.trim() || body.customerName.trim().length < 2) {
      return problem(400, "Ugyldig forespørsel", "customerName må være minst 2 tegn.");
    }

    try {
      const order = createMockOrder(body);
      emitMockOrderCreated(order);
      return HttpResponse.json(order, {
        status: 201,
        headers: { location: `/orders/${order.orderId}` },
      });
    } catch {
      return problem(404, "Ikke funnet", `Fant ingen kaffe med id ${body.coffeeId}.`);
    }
  }),

  http.get(`${KAFFEBAR_API_URL}/orders/:orderId`, ({ params }) => {
    const order = findMockOrder(String(params.orderId));
    return order
      ? HttpResponse.json(order)
      : problem(404, "Ikke funnet", `Fant ingen bestilling med id ${params.orderId}.`);
  }),

  http.patch(`${KAFFEBAR_API_URL}/orders/:orderId`, async ({ params, request }) => {
    const { status } = (await request.json()) as { status: OrderStatus };
    const result = setMockStatus(String(params.orderId), status);
    if (!result) {
      return problem(404, "Ikke funnet", `Fant ingen bestilling med id ${params.orderId}.`);
    }
    if (result.previousStatus !== status) {
      emitMockStatusChanged(result.order, result.previousStatus);
    }
    return HttpResponse.json(result.order);
  }),
];
