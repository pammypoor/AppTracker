import {useEffect, useState} from 'react';

export const UseDarkMode = () => {
    const [theme, setTheme] = useState('light');
    
    const setMode = mode => {
        window.localStorage.setItem('appTracker.Theme', mode)
        setTheme(mode)
    }

    const themeToggler = () => {
        theme === 'light'? setMode('dark') : setMode('light')
    };

    useEffect(() => {
        const localTheme = window.localStorage.getItem('appTracker.Theme');
        localTheme && setTheme(localTheme)
    }, []);

    return [theme, themeToggler]
};