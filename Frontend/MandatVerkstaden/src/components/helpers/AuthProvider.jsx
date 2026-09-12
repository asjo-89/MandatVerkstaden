import { useState, useEffect } from 'react'
import { AuthContext } from '../contexts/AuthContext'
import API_URL from '../../ApiUrl';
import { ApiFetch } from './ApiFetch';

export const AuthProvider = ({ children }) => {

    const [user, setUser] = useState(null);
    const [error, setError] = useState(null);   
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchUser = async () => {
            setError(null);
            try { 
                const response = await fetch(`${API_URL}/auth/current-user`, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    credentials: "include"
                });
                
                if(!response.ok) {
                    return;
                }
                const data = await response.json();
                
                setUser(data);
            } catch (catchedError) {
                setError(catchedError.message || "Kunde inte hämta användaren. Vänligen försök igen senare.");
            } finally {
                setLoading(false);
            }
        };
        fetchUser();
    }, []);
    
    const login = async (username, password) => {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                username: username,
                password: password
            }),
            credentials: "include"
        });
        
        if(!response.ok) {
            const data = await response.json();
            setError(data.message || "Inloggning misslyckades. Vänligen försök igen.");
            return false;
        } 
        const data = await response.json();

        setUser(data);
        setError(null);
        return true;
    };

    const logout = async () => {
        try {
            await ApiFetch(
                `${API_URL}/auth/logout`,
                { method: "POST" },
                false
            );
            setError(null);
        } catch (catchedError) {
            setError(catchedError.message || "Kunde inte logga ut. Vänligen försök igen.");
        } finally {
            setUser(null);
        }
    }

    const refresh = async () => {
        const response = await fetch(`${API_URL}/auth/refresh`, {
            method: "POST",
            credentials: "include"
        });

        if(!response.ok) {
            setUser(null);
            setError("Kunde inte uppdatera användaren. Vänligen försök igen senare.");
            return false;
        }
        setError(null);
        return true; 
    }


  return (
    <AuthContext.Provider value={{ user, error, loading, login, logout, refresh}}>
      {children}
    </AuthContext.Provider>
  )
}
