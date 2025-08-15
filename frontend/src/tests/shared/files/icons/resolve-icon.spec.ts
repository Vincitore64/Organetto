import { resolveIconCategory } from '@/shared'
import { expect, it } from 'vitest'

it('image by ext', () => { expect(resolveIconCategory('', 'x.jpg')).toBe('image') })
it('pdf by mime',  () => { expect(resolveIconCategory('application/pdf','x.bin')).toBe('pdf') })