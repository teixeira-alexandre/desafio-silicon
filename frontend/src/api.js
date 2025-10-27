const baseUrl = import.meta.env.VITE_API || 'http://localhost:5000'

export async function getSaldo(idConta){
  const r = await fetch(`${baseUrl}/api/contas/${idConta}/saldo`)
  return r.json()
}

export async function postDeposito(idConta, valor){
  const r = await fetch(`${baseUrl}/api/contas/${idConta}/deposito`, {
    method:'POST',
    headers:{'Content-Type':'application/json'},
    body: JSON.stringify({ valor })
  })
  return r.json()
}

export async function postSaque(idConta, valor){
  const r = await fetch(`${baseUrl}/api/contas/${idConta}/saque`, {
    method:'POST',
    headers:{'Content-Type':'application/json'},
    body: JSON.stringify({ valor })
  })
  return r.json()
}

export async function postBloqueio(idConta){
  const r = await fetch(`${baseUrl}/api/contas/${idConta}/bloqueio`, { method:'POST' })
  return r.json()
}

export async function getExtrato(idConta, de, ate){
  const p = new URLSearchParams()
  if(de) p.set('de', de)
  if(ate) p.set('ate', ate)
  const r = await fetch(`${baseUrl}/api/contas/${idConta}/extrato?${p.toString()}`)
  return r.json()
}
