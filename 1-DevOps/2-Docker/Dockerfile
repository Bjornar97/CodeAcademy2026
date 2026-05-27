FROM node:20-alpine as deps

RUN apk add libc6-compat
WORKDIR /app
RUN corepack enable
COPY package.json yarn.lock ./
RUN yarn install --immutable


FROM node:20-alpine as builder
WORKDIR /app
RUN corepack enable
ARG NEXT_PUBLIC_ENABLE_IDEM_FORM=false
ARG NEXT_PUBLIC_SHOW_SEEDED_IDEMS=false
ENV NEXT_PUBLIC_ENABLE_IDEM_FORM=${NEXT_PUBLIC_ENABLE_IDEM_FORM}
ENV NEXT_PUBLIC_SHOW_SEEDED_IDEMS=${NEXT_PUBLIC_SHOW_SEEDED_IDEMS}

COPY --from=deps /app/node_modules /app/node_modules
COPY --from=deps /app/.yarn /app/.yarn

RUN yarn build


FROM node:20-alpine as runner
WORKDIR /app
ENV NODE_ENV=production
ENV NEXT_TELEMETRY_DISABLED=1

RUN addgroup -g 1001 nodejs && adduser -D -u 1001 -G nodejs nextjs

COPY --chown=nextjs:nodejs --from=builder /app/public ./public
COPY --chown=nextjs:nodejs --from=builder /app/.next/standalone ./
COPY --chown=nextjs:nodejs --from=builder /app/.next/static ./.next/static

USER nextjs

EXPOSE 3000
CMD ["node", "server.js"]