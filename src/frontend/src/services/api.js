const BASE_URL = '/api'

async function request(endpoint, options={}){
    const headers = new Headers(options.headers)

    if(options.body != null && !(options.body instanceof FormData))
        headers.set('Content-Type', 'application/json')

    //const token = getAccessToken()
    //if(token)
    //    headers.set('Authorization', `Bearer ${token}`)

    const response = await fetch(`${BASE_URL}${endpoint}`, {
        ...options,
        headers
    })

    if(!response.ok)
        throw new Error(`API Request failed: ${response.status}`)

    const contentType = response.headers.get('content-type') ?? ''
    if(contentType.includes('application/json'))
        return await response.json()

    return await response.text()
}

export const api = {
    get(endpoint) {
        return request(endpoint, { method: 'GET' })
    },

    post(endpoint, data){
        console.log(JSON.stringify(data))

        return request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data)
        })
    },

    put(endpoint, data){
        return request(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data)
        })
    },

    patch(endpoint, data){
        return request(endpoint, {
            method: 'PATCH',
            body: JSON.stringify(data)
        })
    },
    
    delete(endpoint){
        return request(endpoint, { method: 'DELETE' })
    }
}