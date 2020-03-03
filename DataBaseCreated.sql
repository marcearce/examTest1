-- Database: examTest1

-- DROP DATABASE "examTest1";

CREATE DATABASE "examTest1"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Spanish_Argentina.1252'
    LC_CTYPE = 'Spanish_Argentina.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Table: public.log

-- DROP TABLE public.log;

CREATE TABLE public.log
(
    id integer NOT NULL DEFAULT nextval('log_id_seq'::regclass),
    message character varying(255) COLLATE pg_catalog."default" NOT NULL,
    type_message boolean NOT NULL,
    type_warning boolean NOT NULL,
    type_error boolean NOT NULL,
    created date NOT NULL,
    CONSTRAINT log_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.log
    OWNER to postgres;

COMMENT ON COLUMN public.log.message
    IS 'mensaje';

COMMENT ON COLUMN public.log.type_message
    IS 'tipo mensaje';

COMMENT ON COLUMN public.log.type_warning
    IS 'tipo alerta';

COMMENT ON COLUMN public.log.type_error
    IS 'tipo error';

COMMENT ON COLUMN public.log.created
    IS 'creacion del registro ';