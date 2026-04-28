# SchoolAPI

API REST para gestão escolar com suporte a dois segmentos: **Maternal** e **Fundamental**. Desenvolvida em .NET 8 com Entity Framework Core e PostgreSQL.

---

## Sumário

- [Visão geral](#visão-geral)
- [Como rodar](#como-rodar)
- [Autenticação](#autenticação)
- [Roles e permissões](#roles-e-permissões)
- [Enums](#enums)
- [Endpoints](#endpoints)
  - [Auth](#auth)
  - [User](#user)
  - [AnoLetivo](#anoletivo)
  - [Turma](#turma)
  - [Disciplina](#disciplina)
  - [Aluno](#aluno)
  - [TurmaDisciplinaProfessor](#turmadisciplinaprofessor)
  - [Frequencia](#frequencia)
  - [Nota](#nota)
  - [DesenvolvimentoMaternal](#desenvolvimentomaternal)
  - [Diario](#diario)
- [Regras de negócio](#regras-de-negócio)

---

## Visão geral

| Item | Valor |
|---|---|
| Base URL (local) | `http://localhost:5009` |
| Swagger | `http://localhost:5009/swagger` |
| Formato | JSON |
| Autenticação | JWT Bearer |
| Banco de dados | PostgreSQL |

---

## Como rodar

**Pré-requisitos:** .NET 8 SDK, Docker (para o PostgreSQL)

```bash
# Subir o banco (Docker Compose está em SchoolAPI/)
cd SchoolAPI
docker compose up -d

# Aplicar migrations
dotnet ef database update

# Rodar a API
dotnet run --launch-profile http
```

A API sobe em `http://localhost:5009`.

**Usuário padrão (seed):**
```
Email:    admin@escola.com
Senha:    admin123
Role:     Admin
```

---

## Autenticação

Todos os endpoints (exceto `/api/auth/login`) exigem token JWT no header:

```
Authorization: Bearer {token}
```

O token é obtido via login e expira em **8 horas**.

O token JWT contém os claims: `id`, `email`, `role`, `nome`.

---

## Roles e permissões

| Role | Valor numérico | Descrição |
|---|---|---|
| `Admin` | 0 | Acesso total |
| `Diretor` | 1 | Visualização e relatórios |
| `Coordenador` | 2 | Visualização e relatórios |
| `Professor` | 3 | Lançamento de notas e frequências |
| `Aluno` | 4 | Apenas consulta própria |

---

## Enums

### SegmentoEnum (Turma)

| Nome | Valor |
|---|---|
| `Maternal` | 0 |
| `Fundamental` | 1 |

### RoleEnum (User)

| Nome | Valor |
|---|---|
| `Admin` | 0 |
| `Diretor` | 1 |
| `Coordenador` | 2 |
| `Professor` | 3 |
| `Aluno` | 4 |

---

## Endpoints

---

### Auth

#### `POST /api/auth/login`

Autentica um usuário e retorna o token JWT. Endpoint público (sem autenticação).

**Request body:**
```json
{
  "email": "admin@escola.com",
  "password": "admin123"
}
```

**Response `200`:**
```json
{
  "token": "eyJhbGci...",
  "userName": "Administrador",
  "userEmail": "admin@escola.com",
  "userRole": 0,
  "userId": 1
}
```

**Response `404`:** Email ou senha inválidos.

---

### User

> Todos os endpoints exigem role **Admin**.

#### `GET /api/user`

Lista todos os usuários com paginação.

**Query params:**

| Param | Tipo | Padrão | Descrição |
|---|---|---|---|
| `page` | int | 1 | Página atual |
| `pageSize` | int | 10 | Itens por página |

**Response `200`:**
```json
{
  "data": [
    {
      "id": 1,
      "nome": "Administrador",
      "email": "admin@escola.com",
      "role": 0,
      "ativo": true
    }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 10
}
```

#### `GET /api/user/{id}`

Retorna um usuário pelo ID.

**Response `200`:**
```json
{
  "id": 2,
  "nome": "Prof. João",
  "email": "joao@escola.com",
  "role": 3,
  "ativo": true
}
```

**Response `404`:** Usuário não encontrado.

#### `POST /api/user`

Cria um novo usuário.

**Request body:**
```json
{
  "nome": "Prof. João",
  "email": "joao@escola.com",
  "password": "senha123",
  "role": 3
}
```

**Response `201`:**
```json
{
  "id": 2,
  "nome": "Prof. João",
  "email": "joao@escola.com",
  "role": 3,
  "ativo": true
}
```

#### `PUT /api/user/{id}`

Atualiza um usuário existente. Mesmo body do POST.

**Response `200`:** Objeto do usuário atualizado.  
**Response `404`:** Usuário não encontrado.

#### `DELETE /api/user/{id}`

Desativa um usuário (soft delete).

**Response `204`:** Sem conteúdo.  
**Response `404`:** Usuário não encontrado.

---

### AnoLetivo

> Todos os endpoints exigem role **Admin**.

#### `GET /api/anoletivo`

Lista todos os anos letivos com paginação.

**Query params:** `page`, `pageSize` (mesmos padrões acima)

**Response `200`:**
```json
{
  "data": [
    { "id": 1, "ano": 2026, "ativo": true }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 10
}
```

#### `GET /api/anoletivo/ativo`

Retorna o ano letivo atualmente ativo.

**Response `200`:**
```json
{ "id": 1, "ano": 2026, "ativo": true }
```

**Response `404`:** Nenhum ano letivo ativo.

#### `GET /api/anoletivo/{id}`

Retorna um ano letivo pelo ID.

**Response `200`:** `{ "id": 1, "ano": 2026, "ativo": true }`  
**Response `404`:** Não encontrado.

#### `POST /api/anoletivo`

**Request body:**
```json
{ "ano": 2026 }
```

**Response `201`:** `{ "id": 1, "ano": 2026, "ativo": true }`

#### `PUT /api/anoletivo/{id}`

Mesmo body do POST.

**Response `200`:** Objeto atualizado.  
**Response `404`:** Não encontrado.

#### `DELETE /api/anoletivo/{id}`

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### Turma

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| GET | Admin, Diretor, Coordenador |
| POST, PUT, DELETE | Admin |

#### `GET /api/turma`

Lista todas as turmas ativas com paginação.

**Query params:** `page`, `pageSize`

**Response `200`:**
```json
{
  "data": [
    {
      "id": 1,
      "nome": "Turma A Maternal",
      "segmento": 0,
      "anoLetivoId": 1,
      "anoLetivoAno": "2026",
      "ativo": true
    }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 10
}
```

#### `GET /api/turma/ano-letivo/{anoLetivoId}`

Lista todas as turmas de um determinado ano letivo.

**Response `200`:** Array de turmas (mesmo formato acima, sem paginação).

#### `GET /api/turma/{id}`

**Response `200`:** Objeto da turma.  
**Response `404`:** Não encontrado.

#### `POST /api/turma`

**Request body:**
```json
{
  "nome": "1o Ano A",
  "segmento": 1,
  "anoLetivoId": 1
}
```

> `segmento`: `0` = Maternal, `1` = Fundamental

**Response `201`:** Objeto da turma criada.

#### `PUT /api/turma/{id}`

Mesmo body do POST.

**Response `200`:** Objeto atualizado.  
**Response `404`:** Não encontrado.

#### `DELETE /api/turma/{id}`

Soft delete (marca `ativo = false`).

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### Disciplina

> GET: qualquer usuário autenticado. POST, PUT, DELETE: somente **Admin**.

#### `GET /api/disciplina`

**Query params:** `page`, `pageSize`

**Response `200`:**
```json
{
  "data": [
    { "id": 1, "nome": "Matemática", "ativo": true },
    { "id": 2, "nome": "Português", "ativo": true }
  ],
  "total": 2,
  "page": 1,
  "pageSize": 10
}
```

#### `GET /api/disciplina/{id}`

**Response `200`:** `{ "id": 1, "nome": "Matemática", "ativo": true }`  
**Response `404`:** Não encontrado.

#### `POST /api/disciplina`

**Request body:** `{ "nome": "Matemática" }`

**Response `201`:** `{ "id": 1, "nome": "Matemática", "ativo": true }`

#### `PUT /api/disciplina/{id}`

**Request body:** `{ "nome": "Matemática" }`

**Response `200`:** Objeto atualizado.  
**Response `404`:** Não encontrado.

#### `DELETE /api/disciplina/{id}`

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### Aluno

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| `GET /` (todos) | Admin, Diretor, Coordenador |
| `GET /turma/{id}` | Admin, Diretor, Coordenador, Professor |
| `GET /{id}` | Qualquer autenticado |
| POST, PUT, DELETE | Admin |

#### `GET /api/aluno`

Lista todos os alunos ativos com paginação.

**Query params:** `page`, `pageSize`

**Response `200`:**
```json
{
  "data": [
    {
      "id": 1,
      "nome": "Maria Souza",
      "numeroChamada": 1,
      "dataNascimento": "2021-03-15",
      "turmaId": 1,
      "turmaNome": "Turma A Maternal",
      "anoLetivoId": 1,
      "anoLetivo": 2026,
      "userId": 3,
      "ativo": true
    }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 10
}
```

#### `GET /api/aluno/turma/{turmaId}`

Lista todos os alunos ativos de uma turma, ordenados por número de chamada.

**Response `200`:** Array de alunos (mesmo formato, sem paginação).

#### `GET /api/aluno/{id}`

**Response `200`:** Objeto do aluno.  
**Response `404`:** Não encontrado.

#### `POST /api/aluno`

**Request body:**
```json
{
  "nome": "Maria Souza",
  "numeroChamada": 1,
  "dataNascimento": "2021-03-15",
  "turmaId": 1,
  "anoLetivoId": 1,
  "userId": 3
}
```

> `dataNascimento`: formato `YYYY-MM-DD`  
> `userId`: ID do usuário do sistema vinculado ao aluno (deve ter `role: 4`)

**Response `201`:** Objeto do aluno criado.

#### `PUT /api/aluno/{id}`

Mesmo body do POST.

**Response `200`:** Objeto atualizado.  
**Response `404`:** Não encontrado.

#### `DELETE /api/aluno/{id}`

Soft delete.

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### TurmaDisciplinaProfessor

Vínculo entre uma turma, uma disciplina, um professor e um ano letivo. A combinação `(turmaId + disciplinaId + anoLetivoId)` é única.

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| GET | Admin, Diretor, Coordenador |
| `GET /professor/{id}` | Admin, Diretor, Coordenador, Professor |
| POST, DELETE | Admin |

#### `GET /api/turmadisciplinaprofessor/turma/{turmaId}`

Lista todos os vínculos de uma turma.

**Response `200`:**
```json
[
  {
    "id": 1,
    "turmaId": 2,
    "turmaNome": "1o Ano A",
    "disciplinaId": 1,
    "disciplinaNome": "Matemática",
    "professorId": 2,
    "professorNome": "Prof. João",
    "anoLetivoId": 1,
    "anoLetivo": 2026
  }
]
```

#### `GET /api/turmadisciplinaprofessor/professor/{professorId}`

Lista todas as turmas/disciplinas de um professor.

**Response `200`:** Array com o mesmo formato acima.

#### `GET /api/turmadisciplinaprofessor/{id}`

**Response `200`:** Objeto do vínculo.  
**Response `404`:** Não encontrado.

#### `POST /api/turmadisciplinaprofessor`

**Request body:**
```json
{
  "turmaId": 2,
  "disciplinaId": 1,
  "professorId": 2,
  "anoLetivoId": 1
}
```

**Response `201`:** Objeto do vínculo criado.

#### `DELETE /api/turmadisciplinaprofessor/{id}`

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### Frequencia

Registro de presença/ausência por turma e data. Suporta **upsert**: se já existe frequência para a mesma turma e data, os dados são atualizados.

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| GET | Admin, Diretor, Coordenador, Professor |
| POST | Admin, Professor |
| `GET /aluno/...` (percentual) | Qualquer autenticado |

#### `GET /api/frequencia/turma/{turmaId}/ano/{anoLetivoId}`

Lista todas as frequências de uma turma no ano letivo, ordenadas por data.

**Response `200`:**
```json
[
  {
    "id": 1,
    "turmaId": 2,
    "data": "2026-04-27",
    "alunos": [
      { "alunoId": 2, "alunoNome": "Pedro Lima", "presente": true }
    ]
  },
  {
    "id": 2,
    "turmaId": 2,
    "data": "2026-04-28",
    "alunos": [
      { "alunoId": 2, "alunoNome": "Pedro Lima", "presente": false }
    ]
  }
]
```

#### `GET /api/frequencia/turma/{turmaId}/data/{data}`

Retorna a frequência de uma turma em uma data específica.

> `data`: formato `YYYY-MM-DD`

**Response `200`:** Objeto de frequência.  
**Response `404`:** Frequência não registrada nessa data.

#### `GET /api/frequencia/{id}`

**Response `200`:** Objeto de frequência.  
**Response `404`:** Não encontrado.

#### `GET /api/frequencia/aluno/{alunoId}/turma/{turmaId}/ano/{anoLetivoId}`

Calcula o percentual de frequência de um aluno.

**Response `200`:**
```json
{
  "alunoId": 2,
  "turmaId": 2,
  "anoLetivoId": 1,
  "frequencia": 50.0
}
```

#### `POST /api/frequencia`

Lança ou atualiza a frequência de uma turma em uma data. Se a data já tiver registro, faz upsert dos alunos.

**Request body:**
```json
{
  "turmaId": 2,
  "anoLetivoId": 1,
  "data": "2026-04-27",
  "alunos": [
    { "alunoId": 2, "presente": true },
    { "alunoId": 3, "presente": false }
  ]
}
```

**Response `200`:**
```json
{
  "id": 1,
  "turmaId": 2,
  "data": "2026-04-27",
  "alunos": [
    { "alunoId": 2, "alunoNome": "Pedro Lima", "presente": true },
    { "alunoId": 3, "alunoNome": "Ana Costa", "presente": false }
  ]
}
```

---

### Nota

As notas são organizadas em **unidades**:

| Unidade | Descrição |
|---|---|
| 1, 2, 3 | 1º semestre |
| 4, 5, 6 | 2º semestre |
| 7 | Recuperação Final |

Regras de cálculo:
- **Média 1º semestre** = (U1 + U2 + U3) / 3
- **Média 2º semestre** = (U4 + U5 + U6) / 3
- **Média anual** = (Média1Sem + Média2Sem) / 2
- **Média final** = (MediaAnual + Recuperação) / 2 (se média anual < 6)
- **Resultado**: `"Aprovado"` se média final ≥ 6, `"Reprovado"` caso contrário

O lançamento faz **upsert**: se já existe nota para o mesmo aluno/disciplina/unidade/ano, ela é atualizada.

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| `GET /aluno/...` | Qualquer autenticado |
| `GET /turma/...` | Admin, Diretor, Coordenador, Professor |
| `GET /media/...` | Qualquer autenticado |
| POST (lançar) | Admin, Professor |
| DELETE | Admin |

#### `GET /api/nota/aluno/{alunoId}/ano/{anoLetivoId}`

Lista todas as notas de um aluno no ano letivo.

**Response `200`:**
```json
[
  {
    "id": 1,
    "alunoId": 2,
    "alunoNome": "Pedro Lima",
    "disciplinaId": 1,
    "disciplinaNome": "Matemática",
    "turmaId": 2,
    "unidade": 1,
    "valor": 7.5
  }
]
```

#### `GET /api/nota/turma/{turmaId}/disciplina/{disciplinaId}/ano/{anoLetivoId}`

Lista as notas de todos os alunos de uma turma em uma disciplina.

**Response `200`:** Array de notas (mesmo formato acima).

#### `GET /api/nota/media/aluno/{alunoId}/disciplina/{disciplinaId}/ano/{anoLetivoId}`

Retorna as notas e médias calculadas de um aluno em uma disciplina.

**Response `200`:**
```json
{
  "alunoId": 2,
  "disciplinaId": 1,
  "unidade1": 7.5,
  "unidade2": 8.0,
  "unidade3": 6.0,
  "media1Semestre": 7.17,
  "unidade4": 9.0,
  "unidade5": null,
  "unidade6": null,
  "media2Semestre": null,
  "mediaAnual": null,
  "notaRecuperacao": null,
  "mediaFinal": null,
  "resultado": null
}
```

> Campos com `null` indicam que a unidade ainda não foi lançada ou a média não pode ser calculada.

#### `POST /api/nota`

Lança ou atualiza uma nota.

**Request body:**
```json
{
  "alunoId": 2,
  "disciplinaId": 1,
  "turmaId": 2,
  "anoLetivoId": 1,
  "unidade": 1,
  "valor": 7.5
}
```

> `unidade`: 1 a 7  
> `valor`: 0.0 a 10.0

**Response `200`:**
```json
{
  "id": 1,
  "alunoId": 2,
  "alunoNome": "Pedro Lima",
  "disciplinaId": 1,
  "disciplinaNome": "Matemática",
  "turmaId": 2,
  "unidade": 1,
  "valor": 7.5
}
```

**Response `400`:** Valor ou unidade inválidos:
```json
{ "message": "Unidade deve ser entre 1 e 7." }
{ "message": "Nota deve ser entre 0 e 10." }
```

#### `DELETE /api/nota/{id}`

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### DesenvolvimentoMaternal

Avaliação descritiva por bimestre, exclusiva para turmas do segmento **Maternal**. O lançamento faz **upsert**: se já existe registro para o mesmo aluno/bimestre/ano, a descrição é atualizada.

#### Permissões por ação

| Ação | Roles permitidas |
|---|---|
| GET | Qualquer autenticado |
| POST | Admin, Professor |
| DELETE | Admin |

#### `GET /api/desenvolvimentomaternal/aluno/{alunoId}/ano/{anoLetivoId}`

Lista todos os registros de desenvolvimento de um aluno no ano.

**Response `200`:**
```json
[
  {
    "id": 1,
    "alunoId": 1,
    "alunoNome": "Maria Souza",
    "turmaId": 1,
    "turmaNome": "Turma A Maternal",
    "anoLetivoId": 1,
    "bimestre": 1,
    "descricao": "Aluna demonstra boa adaptação ao ambiente escolar."
  }
]
```

#### `GET /api/desenvolvimentomaternal/aluno/{alunoId}/bimestre/{bimestre}/ano/{anoLetivoId}`

Retorna o desenvolvimento de um aluno em um bimestre específico.

**Response `200`:** Objeto de desenvolvimento.  
**Response `404`:** Não encontrado.

#### `GET /api/desenvolvimentomaternal/{id}`

**Response `200`:** Objeto de desenvolvimento.  
**Response `404`:** Não encontrado.

#### `POST /api/desenvolvimentomaternal`

Lança ou atualiza o desenvolvimento de um bimestre.

**Request body:**
```json
{
  "alunoId": 1,
  "turmaId": 1,
  "anoLetivoId": 1,
  "bimestre": 1,
  "descricao": "Aluna demonstra boa adaptação ao ambiente escolar."
}
```

> `bimestre`: 1 a 4

**Response `200`:** Objeto do desenvolvimento salvo.

**Response `400`:**
```json
{ "message": "Bimestre deve ser entre 1 e 4." }
```

#### `DELETE /api/desenvolvimentomaternal/{id}`

**Response `204`:** Sem conteúdo.  
**Response `404`:** Não encontrado.

---

### Diario

Endpoint agregador que retorna o boletim completo de um aluno — frequência mensal e desempenho acadêmico — em uma única chamada. Varia conforme o segmento da turma.

> Qualquer usuário autenticado pode acessar.

#### `GET /api/diario/maternal/aluno/{alunoId}/ano/{anoLetivoId}`

Retorna o diário de um aluno do segmento **Maternal**.

**Response `200`:**
```json
{
  "alunoId": 1,
  "alunoNome": "Maria Souza",
  "numeroChamada": 1,
  "turmaNome": "Turma A Maternal",
  "anoLetivo": 2026,
  "frequencia": {
    "meses": [
      {
        "mes": 4,
        "mesNome": "Abril",
        "dias": {
          "7": true,
          "8": false,
          "9": true
        },
        "totalFaltas": 1
      }
    ],
    "totalFaltas": 1,
    "frequencia": 66.67
  },
  "desenvolvimentos": [
    {
      "bimestre": 1,
      "descricao": "Aluna demonstra boa adaptação ao ambiente escolar."
    },
    {
      "bimestre": 2,
      "descricao": "Evoluiu na coordenação motora e linguagem oral."
    }
  ]
}
```

> `frequencia.meses[].dias`: objeto onde a **chave** é o dia do mês (`"7"`, `"8"`, ...) e o **valor** é `true` (presente), `false` (ausente) ou `null` (sem aula registrada). Dias sem chave = sem registro.

**Response `404`:** Aluno não encontrado.

#### `GET /api/diario/fundamental/aluno/{alunoId}/ano/{anoLetivoId}`

Retorna o diário de um aluno do segmento **Fundamental**.

**Response `200`:**
```json
{
  "alunoId": 2,
  "alunoNome": "Pedro Lima",
  "numeroChamada": 1,
  "turmaNome": "1o Ano A",
  "anoLetivo": 2026,
  "frequencia": {
    "meses": [
      {
        "mes": 4,
        "mesNome": "Abril",
        "dias": {
          "27": true,
          "28": false
        },
        "totalFaltas": 1
      }
    ],
    "totalFaltas": 1,
    "frequencia": 50.0
  },
  "notas": [
    {
      "disciplinaId": 1,
      "disciplinaNome": "Matemática",
      "unidade1": 7.5,
      "unidade2": 8.0,
      "unidade3": 6.0,
      "media1Semestre": 7.17,
      "unidade4": 9.0,
      "unidade5": null,
      "unidade6": null,
      "media2Semestre": null,
      "mediaAnual": null,
      "notaRecuperacao": null,
      "mediaFinal": null,
      "resultado": null
    },
    {
      "disciplinaId": 2,
      "disciplinaNome": "Português",
      "unidade1": 4.0,
      "unidade2": 5.0,
      "unidade3": 5.5,
      "media1Semestre": 4.83,
      "unidade4": null,
      "unidade5": null,
      "unidade6": null,
      "media2Semestre": null,
      "mediaAnual": null,
      "notaRecuperacao": null,
      "mediaFinal": null,
      "resultado": null
    }
  ],
  "observacoes": null,
  "resultado": null
}
```

> `resultado` (raiz): `"Aprovado"`, `"Reprovado"` ou `null` (ano incompleto). Será `"Reprovado"` se qualquer disciplina estiver reprovada.

**Response `404`:** Aluno não encontrado.

---

## Regras de negócio

### Fluxo de cadastro recomendado

A ordem abaixo garante que as dependências existam antes de cada cadastro:

```
1. AnoLetivo     → cria o ano letivo
2. Turma         → vinculada ao AnoLetivo
3. Disciplina    → independente
4. User          → criar usuários (professores, alunos)
5. Aluno         → vinculado a Turma + AnoLetivo + User
6. TurmaDisciplinaProfessor → vínculo turma + disciplina + professor + ano
7. Frequencia    → lançamento diário
8. Nota          → lançamento por unidade
9. DesenvolvimentoMaternal → somente para turmas Maternal
```

### Restrições de unicidade

| Entidade | Índice único |
|---|---|
| `TurmaDisciplinaProfessor` | `(TurmaId, DisciplinaId, AnoLetivoId)` |
| `Frequencia` | `(TurmaId, Data)` |
| `Nota` | `(AlunoId, DisciplinaId, Unidade, AnoLetivoId)` |
| `DesenvolvimentoMaternal` | `(AlunoId, Bimestre, AnoLetivoId)` |

### Upsert automático

Os seguintes endpoints fazem upsert em vez de retornar erro ao duplicar:

- `POST /api/frequencia` — se já existe frequência para turma + data, atualiza as presenças
- `POST /api/nota` — se já existe nota para aluno + disciplina + unidade + ano, atualiza o valor
- `POST /api/desenvolvimentomaternal` — se já existe registro para aluno + bimestre + ano, atualiza a descrição

### Soft delete

As entidades `User`, `Turma` e `Aluno` usam soft delete: o campo `ativo` é marcado como `false`. As consultas filtram automaticamente por `ativo = true`.

### Cálculo de médias (Fundamental)

```
Média 1º sem  = (U1 + U2 + U3) / 3
Média 2º sem  = (U4 + U5 + U6) / 3
Média anual   = (Média1Sem + Média2Sem) / 2

Se média anual >= 6  → Aprovado, média final = média anual
Se média anual < 6   → vai para recuperação (U7)
  Média final = (média anual + U7) / 2
  Se média final >= 6 → Aprovado
  Se média final < 6  → Reprovado

Resultado geral do aluno = Reprovado se qualquer disciplina reprovada
```

### Paginação

Os endpoints de listagem (`GET /api/{recurso}`) usam paginação com o seguinte formato de resposta:

```json
{
  "data": [...],
  "total": 42,
  "page": 1,
  "pageSize": 10
}
```
