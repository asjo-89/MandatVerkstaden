export const ApiFetch = async (apiUrl, options = {}, refresh) => {

    const response = await fetch(apiUrl, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...(options.headers || {})
        },
        body: options.body ? JSON.stringify(options.body) : null,
        credentials: "include"
    });

    if(response.status === 401 && refresh) {
        const refreshResponse = await refresh();
        
        if(refreshResponse) {
            return await ApiFetch(apiUrl, options, false);
        }
    }

    let data = null;
    try {
        data = await response.json();
    } catch {
        // Ignoring JSON parsing error because the response might not be JSON
    }
    
    if(!response.ok) {
        console.log("API Error:", data);
        const error = new Error(data?.message || "Ett fel uppstod. Vänligen försök igen.");
        error.errors = data?.errors || null;
        throw error;
    }
    return data;
}
