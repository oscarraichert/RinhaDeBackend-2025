CREATE UNLOGGED TABLE IF NOT EXISTS public."Payments"
(
    "correlationId" uuid NOT NULL,
    amount numeric NOT NULL,
    "requestedAt" timestamp with time zone NOT NULL,
    "processedOnFallback" boolean NOT NULL DEFAULT false,
    CONSTRAINT "PK_Payments" PRIMARY KEY ("correlationId")
);

-- Create index to speed up filtering by requestedAt
CREATE INDEX IF NOT EXISTS idx_payments_requestedat
ON public."Payments" ("requestedAt");