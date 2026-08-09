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

    const data = await response.json();
    if(!response.ok) {
        throw new Error(data.message || "Ett fel uppstod vid hämtning av data.");
    }
    return data;

}
