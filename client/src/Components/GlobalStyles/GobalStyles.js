import {createGlobalStyle} from "styled-components";

export const GLobalStyles = createGlobalStyle`
    body {
        background: ${({ theme }) => theme.body};
        color: ${({ theme }) => theme.text};
        font-familty: Tahoma, Helvetica, Arial, Roboto, sans-serif;
        transition: all 0.50s linera;
    }
    `
    